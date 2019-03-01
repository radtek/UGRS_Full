SELECT T1.Code, T1.U_IdRise, T1.U_Employee, CONCAT(T2.lastName, ' ', T2.firstName) AS lastName, CASE WHEN T1.U_Status = 1 THEN 'Y' ELSE 'N' END AS Status 
FROM [@UG_TBL_MQ_RIEM] T1
INNER JOIN OHEM T2
	ON T1.U_Employee = T2.empID
WHERE T1.U_IdRise = {RiseId}