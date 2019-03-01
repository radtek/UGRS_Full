SELECT SUM(ISNULL(A1.Debit, 0)) - SUM(ISNULL(A1.Credit, 0)) Adeudo, A1.U_GLO_CodeMov
FROM JDT1 A1
WHERE A1.Account = '{Account}' AND A1.U_GLO_Auxiliar = '{EmployeeId}'
	  AND A1.U_GLO_CodeMov IN (SELECT A2.U_GLO_CodeMov FROM OVPM A2 
							   WHERE A2.Canceled = 'N' AND A2.U_GLO_FolioOri != '{RiseId}')
group by A1.U_GLO_CodeMov
HAVING SUM(ISNULL(A1.Debit, 0)) - SUM(ISNULL(A1.Credit, 0)) > 0