SELECT T1.Code, T1.Name, T1.U_IdRise, T1.U_CreatedDate, T1.U_StartDate, T1.U_EndDate, 
T1.U_Client, T2.CardName, T1.U_Supervisor, CONCAT(T3.lastName, ' ', T3.firstName) AS SupervisorName, T1.U_DocStatus, T1.U_DocRef, T1.U_UserId
FROM [@UG_TBL_MQ_RISE] T1
INNER JOIN OCRD T2
	ON T1.U_Client = T2.CardCode
INNER JOIN OHEM T3
	ON T1.U_Supervisor = T3.empID
 WHERE U_IdRise = '{RiseId}'