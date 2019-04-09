declare @Folio as nvarchar(10);
declare @Account as nvarchar(30);
set @Folio= '{Folio}'
set @Account= '{Account}'

SELECT A1.U_Folio Folio, A2.EmpId EmpId, CONCAT(A3.lastName, ' ', A3.firstName) AS EmployeeName, A2.Import, A2.Account FROM
(
	SELECT *FROM [@UG_TR_CMSN]  with (Nolock) WHERE ISNULL(U_HasDriverCms, 'N') = 'N' AND U_Folio = @Folio AND U_Status = '2' -- Status cerrado
) A1
INNER JOIN
(
	SELECT T1.Ref1, T2.U_GLO_Auxiliar EmpId, T2.Account, SUM(ISNULL(T2.Credit, 0)) - SUM(ISNULL(T2.Debit, 0)) Import
	FROM OJDT T1  with (Nolock)
	INNER JOIN JDT1 T2  with (Nolock)
		ON T1.TransId = T2.TransId
	WHERE T1.TransCode = 'TRCM'
	GROUP BY T1.Ref1, T2.U_GLO_Auxiliar, T2.Account
) A2 ON A2.Ref1 = a1.U_Folio AND A2.Account = @Account
INNER JOIN OHEM A3  with (Nolock) on A3.EmpId = A2.EmpId