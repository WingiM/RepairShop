using FluentValidation;
using LanguageExt.Common;
using LanguageExt.Pipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RepairShop.Services.Impl;

public class RequestService : IRequestService
{
    private readonly ApplicationContext _context;
    private readonly IServiceProvider _serviceProvider;

    public RequestService(ApplicationContext context,
        IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// List repair requests for master
    /// </summary>
    public Result<IEnumerable<RepairRequest>> ListForMaster(int masterId)
    {
        var existingMaster = _context.Users.FirstOrDefault(x => x.Id == masterId);
        if (existingMaster is null || existingMaster.RoleId != (int)Roles.Master)
            return new Result<IEnumerable<RepairRequest>>(new Exception(ValidationErrorMessages.UserIsNotMaster));

        var result = _context.RepairRequests
            .Where(x => x.MasterId == masterId || x.MasterId == null)
            .Include(x => x.StatusHistories.Where(z => z.IsActual))
            .ThenInclude(x => x.Status)
            .Include(x => x.Client)
            .Where(x => x.StatusHistories.First(z => z.IsActual).Status.Id != (int)RequestStatuses.Finished);
        return new Result<IEnumerable<RepairRequest>>(result);
    }

    public Result<IEnumerable<RepairRequest>> ListArchiveForMaster(int masterId)
    {
        var existingMaster = _context.Users.FirstOrDefault(x => x.Id == masterId);
        if (existingMaster is null || existingMaster.RoleId != (int)Roles.Master)
            return new Result<IEnumerable<RepairRequest>>(new Exception(ValidationErrorMessages.UserIsNotMaster));

        var result = _context.RepairRequests
            .Where(x => x.MasterId == masterId || x.MasterId == null)
            .Include(x => x.Client)
            .Where(x => x.StatusHistories.First(z => z.IsActual).Status.Id == (int)RequestStatuses.Finished);
        return new Result<IEnumerable<RepairRequest>>(result);
    }

    /// <summary>
    /// List repair requests for client
    /// </summary>
    public Result<IEnumerable<RepairRequest>> ListForClient(int clientId)
    {
        var existingClient = _context.Users.FirstOrDefault(x => x.Id == clientId);
        if (existingClient is null || existingClient.RoleId != (int)Roles.Client)
            return new Result<IEnumerable<RepairRequest>>(new Exception(ValidationErrorMessages.UserIsNotClient));

        var result = _context.RepairRequests
            .Where(x => x.ClientId == clientId)
            .Include(x => x.StatusHistories.Where(z => z.IsActual))
            .ThenInclude(x => x.Status)
            .Include(x => x.Master)
            .Where(x => x.StatusHistories.First(z => z.IsActual).Status.Id != (int)RequestStatuses.Finished);
        return new Result<IEnumerable<RepairRequest>>(result);
    }

    public Result<IEnumerable<RepairRequest>> ListArchiveForClient(int clientId)
    {
        var existingClient = _context.Users.FirstOrDefault(x => x.Id == clientId);
        if (existingClient is null || existingClient.RoleId != (int)Roles.Client)
            return new Result<IEnumerable<RepairRequest>>(new Exception(ValidationErrorMessages.UserIsNotClient));

        var result = _context.RepairRequests
            .Where(x => x.ClientId == clientId)
            .Include(x => x.Master)
            .Where(x => x.StatusHistories.First(z => z.IsActual).Status.Id == (int)RequestStatuses.Finished);
        return new Result<IEnumerable<RepairRequest>>(result);
    }

    /// <summary>
    /// Get the request info by its id
    /// </summary>
    public Result<RepairRequest?> GetRequestById(int requestId)
    {
        return _context.RepairRequests
            .Include(x => x.Master)
            .Include(x => x.Client)
            .Include(x => x.StatusHistories)
            .ThenInclude(x => x.Status)
            .FirstOrDefault(x => x.Id == requestId);
    }

    /// <summary>
    /// Get the list of status changes for the request
    /// </summary>
    public Result<IEnumerable<StatusHistory>> GetStatusHistoryForRequest(int requestId)
    {
        return new Result<IEnumerable<StatusHistory>>(_context.StatusHistories.Where(x => x.RequestId == requestId)
            .OrderBy(x => x.DateChanged));
    }

    /// <summary>
    /// Create a new repair request
    /// </summary>
    public Result<RepairRequest> Create(CreateRepairRequestDto request)
    {
        var validationResult =
            _serviceProvider.GetRequiredService<IValidator<CreateRepairRequestDto>>().Validate(request);
        if (!validationResult.IsValid)
            return new Result<RepairRequest>(new ValidationException(validationResult.Errors));

        var repairRequest = new RepairRequest
            { ClientId = request.ClientId, Description = request.Description, ShortName = request.ShortName };
        _context.RepairRequests.Add(repairRequest);

        // Set default status to a new request
        var requestStatus = new StatusHistory
        {
            DateChanged = DateTime.Now.ToUniversalTime(),
            IsActual = true,
            Request = repairRequest,
            StatusId = (int)RequestStatuses.AwaitsConfirmation
        };
        _context.StatusHistories.Add(requestStatus);

        _context.SaveChanges();
        return repairRequest;
    }

    /// <summary>
    /// Update the name or description of existing repair request
    /// </summary>
    public Result<RepairRequest> Update(UpdateRepairRequestDto request)
    {
        var validationResult =
            _serviceProvider.GetRequiredService<IValidator<UpdateRepairRequestDto>>().Validate(request);
        if (!validationResult.IsValid)
            return new Result<RepairRequest>(new ValidationException(validationResult.Errors));

        var existingRequest = _context.RepairRequests.Find(request.RequestId)!;
        existingRequest.ShortName = request.ShortName;
        existingRequest.Description = request.Description;
        _context.RepairRequests.Update(existingRequest);
        _context.SaveChanges();
        return existingRequest;
    }

    /// <summary>
    /// Assigns the master to the created request
    /// </summary>
    public Result<bool> AssignMaster(AssignMasterToRequestDto request)
    {
        var validationResult = _serviceProvider.GetRequiredService<IValidator<AssignMasterToRequestDto>>()
            .Validate(request);
        if (!validationResult.IsValid)
            return new Result<bool>(new ValidationException(validationResult.Errors));

        var existingRequest = _context.RepairRequests.Find(request.RequestId)!;
        existingRequest.MasterId = request.MasterId;
        _context.RepairRequests.Update(existingRequest);

        ChangeStatusInternal(request.RequestId, RequestStatuses.TransferedToMaster);

        _context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Changes the status of the request
    /// </summary>
    public Result<bool> ChangeStatus(ChangeRequestStatusDto request)
    {
        var validationResult =
            _serviceProvider.GetRequiredService<IValidator<ChangeRequestStatusDto>>().Validate(request);
        if (!validationResult.IsValid)
            return new Result<bool>(new ValidationException(validationResult.Errors));

        return ChangeStatusInternal(request.RequestId, request.Status);
    }

    private Result<bool> ChangeStatusInternal(int requestId, RequestStatuses status)
    {
        var actualStatus = _context.StatusHistories.First(x => x.RequestId == requestId && x.IsActual);
        actualStatus.IsActual = false;
        _context.StatusHistories.Update(actualStatus);

        var requestStatus = new StatusHistory
        {
            DateChanged = DateTime.Now.ToUniversalTime(),
            IsActual = true,
            RequestId = requestId,
            StatusId = (int)status
        };
        _context.StatusHistories.Add(requestStatus);

        _context.SaveChanges();
        _context.ChangeTracker.Clear();
        return true;
    }
}