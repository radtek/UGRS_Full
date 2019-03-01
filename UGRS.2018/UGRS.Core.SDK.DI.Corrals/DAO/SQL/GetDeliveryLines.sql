SELECT  
	B1.LineNum      [LineNum],
	B1.ItemCode     [ItemCode], 
	B1.Price        [Price],   
	B0.DocEntry     [DocEntry], 
	B1.Quantity     [Quantity]
FROM 
	(SELECT CardCode, DocEntry, CANCELED, DocStatus FROM ODLN) B0
	 INNER JOIN  (SELECT   ItemCode, LineNum, Price, TaxCode, OcrCode, Quantity, WhsCode, DocEntry FROM DLN1) B1 ON B1.DocEntry = B0.DocEntry
WHERE 
	B0.CANCELED = 'N' and B0.DocStatus='O' AND CardCode = '{CardCode}' AND B1.WhsCode= '{WhsCode}' 
