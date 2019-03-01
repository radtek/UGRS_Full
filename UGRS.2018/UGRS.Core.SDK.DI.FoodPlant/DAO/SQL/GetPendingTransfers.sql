SELECT DISTINCT
 	 T0.DocEntry                                         [Folio],
	 ISNULL(CONVERT(VARCHAR, T0.DocDate, 103), '')       [DocDate],                
  	 ISNULL(T0.Comments, '')                             [Comments],
	 T0.filler                                           [FromWhs],
	 T0.ToWhsCode                                        [ToWhs]  
FROM 
	OWTR T0
	INNER JOIN WTR1 T1 ON T0.DocEntry = T1.DocEntry 
WHERE
	T1.OcrCode = 'PL_PLANT' AND T0.ToWhsCode = '{Whs}' AND T0.DocStatus = 'O'
