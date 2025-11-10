CREATE Procedure SP_GetAllUsers
as
begin
   SELECT UserID, PersonID, UserName, RoleID, IsActive
FROM   Users
end
go