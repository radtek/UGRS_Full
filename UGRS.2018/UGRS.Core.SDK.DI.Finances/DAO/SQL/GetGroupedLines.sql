SELECT CASE WHEN Currency = 'MXP'
			THEN SUM(LineTotal)
			ELSE SUM(TotalFrgn) 
	   END AS LineTotal,
	   TaxCode, OcrCode, OcrCode2, OcrCode3, U_GLO_BagsBales
FROM INV1
WHERE DocEntry = '{DocEntry}'
GROUP BY TaxCode, OcrCode, OcrCode2, OcrCode3, U_GLO_BagsBales, Currency