SELECT T1.Code, T1.Name, T1.U_Hrs, T1.U_PrcCode, T1.U_IdRise, T1.U_EcoNum, ISNULL(T1.U_Operator, 0) AS U_Operator,
ISNULL(CONCAT(T2.lastName, ' ', T2.firstName), '') AS OperatorName,
ISNULL(T1.U_Supervisor, 0) AS U_Supervisor,
ISNULL(CONCAT(T3.lastName, ' ', T3.firstName), '') AS SupervisorName
FROM [@UG_MQ_RIHT] T1
LEFT JOIN OHEM T2
	ON T1.U_Operator = T2.empID
LEFT JOIN OHEM T3
	ON T1.U_Supervisor = T3.empID
WHERE T1.U_IdRise = '{RiseId}'