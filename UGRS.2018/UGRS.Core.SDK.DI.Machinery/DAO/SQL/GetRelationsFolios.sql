SELECT T1.Code, T1.U_Client, T1.U_CreatedDate, T1.U_DocRef, T1.U_DocStatus, T1.U_IdRise, T1.U_Supervisor, T1.U_UserId, CONCAT (T2.firstName, ' ', T2.lastName) AS ClientName 
FROM [@UG_TBL_MQ_RISE] T1
INNER JOIN OHEM T2
	ON T1.U_Supervisor = T2.empID
WHERE T1.U_DocStatus = 2 AND T1.U_IdRise NOT IN(SELECT T3.U_DocRef FROM [@UG_TBL_MQ_RISE] T3)