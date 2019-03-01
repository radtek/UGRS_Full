SELECT 
    T0.U_RequestId   [ID],
	T0.U_UgrsFolio   [Folio],
	T0.U_UGRSRequest [Type]
FROM 
	dbo.[@UG_PE_WS_PERE] T0
	LEFT JOIN (SELECT U_PE_FolioUGRS, U_PE_RequestCodeUGRS FROM ORDR) T1 ON T0.U_UgrsFolio = T1.U_PE_FolioUGRS AND T1.U_PE_RequestCodeUGRS = T0.U_UgrsRequest
	WHERE T1.U_PE_FolioUGRS IS NULL AND T0.U_RequestId IS NOT NULL 
ORDER BY T0.U_UgrsFolio

