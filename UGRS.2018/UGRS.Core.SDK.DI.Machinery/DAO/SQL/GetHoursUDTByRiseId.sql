SELECT T1.*, CONCAT(T2.lastName, ' ', T2.firstName) AS SupervisorName, CONCAT(T3.lastName, ' ', T3.firstName) AS OperatorName, 
ISNULL(T4.DocEntry, 0) AS DocEntry, ISNULL(T4.DocNum, 0) AS DocNum, ISNULL(T4.CardName, '') AS ClientName, ISNULL(T5.Name, '') AS SectionName
FROM [@UG_MQ_RIHR] T1
INNER JOIN OHEM T2
	ON T1.U_Supervisor = T2.empID
INNER JOIN OHEM T3
	ON T1.U_Operator = T3.empID
LEFT JOIN ORDR T4
	ON T1.U_DocEntry = T4.DocEntry
LEFT JOIN [@UG_GR_ROAD] T5
	ON T5.Code = T1.U_Section
WHERE T1.U_IdRise = '{RiseId}'
