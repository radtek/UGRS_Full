declare @RiseId as nvarchar(10);
declare @Account as nvarchar(30);
declare @EmpId as nvarchar(30);
set @RiseId= '{RiseId}'
set @Account= '{Account}'
set @EmpId = '{EmployeeId}'

SELECT SUM(ISNULL(Debit, 0)) - SUM(ISNULL(Credit, 0)) ImportFS, U_GLO_Auxiliar, Account 
FROM JDT1 with (nolock)
WHERE U_GLO_Auxiliar = @EmpId and Account = @Account and U_GLO_CodeMov IN (SELECT U_GLO_CodeMov FROM OVPM with (nolock) 
								WHERE Canceled = 'N' AND U_GLO_FolioOri = @RiseId) group by U_GLO_Auxiliar, Account 