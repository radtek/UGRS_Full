SELECT DISTINCT
 	 T0.DocEntry                                         [DocEntry],
	 ISNULL(CONVERT(VARCHAR, T0.DocDate, 103), '')       [DocDate],                
  	 ISNULL(T0.Comments, '')                             [Comments],
	 T0.filler                                           [FromWhs],
	 T0.ToWhsCode                                        [ToWhs], 
	 CONVERT(VARCHAR, T0.DocNum)                         [Folio],
	 T0.UserSign                                         [UserID]
FROM 
	OWTR T0
	INNER JOIN (SELECT DocEntry, OcrCode, WhsCode FROM WTR1) T1 ON T0.DocEntry = T1.DocEntry 
	INNER JOIN OWHS T2 ON T2.WhsCode = T1.WhsCode  AND T2.U_GLO_WhsTransit = 'S'
WHERE
	T0.ToWhsCode = '{Whs}' AND T0.U_GLO_Status = 'O' 