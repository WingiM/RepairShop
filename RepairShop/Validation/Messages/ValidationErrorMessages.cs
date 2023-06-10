namespace RepairShop.Validation.Messages;

public static class ValidationErrorMessages
{
    public const string UserDoesNotExist = "Указанного пользователя не существует в системе";
    public const string UserIsNotMaster = "Указанный пользователь не является мастером";
    public const string ClientDoesNotExist = "Такого клиента нет в системе";
    
    public const string PasswordsDoNotMatch = "Пароли не совпадают";
    public const string UserAlreadyExists = "Пользователь с таким логином уже существует";
    public const string LoginCannotBeEmpty = "Логин не может быть пустым";
    public const string LoginCannotBeMoreThan50Symbols = "Максимальная длина логина - 50 символов";
    public const string PasswordCannotBeEmpty = "Пароль не может быть пустым";
    public const string PasswordCannotBeMoreThan50Symbols = "Максимальная длина пароля - 50 символов";
    
    public const string RepairRequestDoesNotExist = "Такого запроса на ремонт не существует";
    public const string RepairRequestStatusAlreadyChanged = "Запрос на ремонт уже переведен в другой статус";
    public const string RepairRequestNameCannotBeEmpty = "Название запроса не может быть пустым";
    public const string RepairRequestNameCannotBeMoreThan50Symbols = "Максимальная длина названия - 50 символов";
    public const string RepairRequestDescriptionCannotBeEmpty = "Описание не может быть пустым";

    public const string RepairRequestDescriptionCannotBeMoreThan1000Symbols =
        "Максимальная длина описания - 1000 символов";

    public const string CannotEditRepairRequest = "Нельзя отредактировать запрос на ремонт, когда он передан мастеру";

    public const string CannotChangeRequestStatus =
        "Можно изменять статусы только для уже переданных мастеру запросов на ремонт";
}