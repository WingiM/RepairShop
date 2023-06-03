using FluentValidation;
using LanguageExt.Common;
using Microsoft.Extensions.DependencyInjection;
using RepairShop.Data;
using RepairShop.Data.DTO;
using RepairShop.Data.Enums;
using RepairShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepairShop.Services.Impl;

public class RequestService : IRequestService
{
    private readonly ApplicationContext _context;
    private readonly IServiceProvider _serviceProdiver;

    public RequestService(ApplicationContext context,
                          IServiceProvider serviceProdiver)
    {
        _context = context;
        _serviceProdiver = serviceProdiver;
    }

    /// <summary>
    /// List repair requests for master
    /// </summary>
    public Result<IEnumerable<RepairRequest>> ListForMaster(int masterId)
    {
        if (_context.Users.FirstOrDefault(x => x.Id == masterId)?.RoleId != (int)Roles.Master)
            return new Result<IEnumerable<RepairRequest>>(new Exception("Указанный пользователь не является мастером"));

        return new Result<IEnumerable<RepairRequest>>(_context.RepairRequests.Where(x => x.MasterId == masterId || x.MasterId == null));
    }

    /// <summary>
    /// List repair requests for client
    /// </summary>
    public Result<IEnumerable<RepairRequest>> ListForClient(int clientId)
    {
        if (_context.Users.FirstOrDefault(x => x.Id == clientId)?.RoleId != (int)Roles.Client)
            return new Result<IEnumerable<RepairRequest>>(new Exception("Указанный пользователь не является клиентом"));

        return new Result<IEnumerable<RepairRequest>>(_context.RepairRequests.Where(x => x.ClientId == clientId));
    }

    /// <summary>
    /// Create a new repair request
    /// </summary>
    public Result<RepairRequest> Create(CreateRepairRequestDto request)
    {
        var validationResult = _serviceProdiver.GetRequiredService<IValidator<CreateRepairRequestDto>>().Validate(request);
        if (!validationResult.IsValid)
            return new Result<RepairRequest>(new ValidationException(validationResult.Errors));

        var repairRequest = new RepairRequest { ClientId = request.ClientId, Description = request.Description, ShortName = request.ShortName };
        _context.RepairRequests.Add(repairRequest);
        _context.SaveChanges();
        return repairRequest;
    }

    /// <summary>
    /// Update the name or description of existing repair request
    /// </summary>
    public Result<RepairRequest> Update(UpdateRepairRequestDto request)
    {
        var validationResult = _serviceProdiver.GetRequiredService<IValidator<UpdateRepairRequestDto>>().Validate(request);
        if (!validationResult.IsValid)
            return new Result<RepairRequest>(new ValidationException(validationResult.Errors));

        var existingRequest = _context.RepairRequests.Find(request.RequestId)!;
        existingRequest.ShortName = request.ShortName;
        existingRequest.Description = request.Description;
        _context.RepairRequests.Update(existingRequest);
        _context.SaveChanges();
        return existingRequest;
    }

    public Result<bool> AssignMaster(int requestId, int masterId)
    {
        throw new NotImplementedException();
    }

    public Result<bool> ChangeStatus(int requestId, RequestStatuses status)
    {
        throw new NotImplementedException();
    }
}
