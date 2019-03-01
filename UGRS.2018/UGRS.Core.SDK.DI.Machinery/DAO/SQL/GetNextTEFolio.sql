SELECT CONCAT('CV_','MQ_MAQUI','_',ISNULL( max(cast(isnull(T0.Folio,'')  as integer)),0)+ 1) AS Folio
FROM(
	SELECT RIGHT(B0.Folio,charindex('_',reverse(B0.Folio))-1) Folio
	FROM
		(
			SELECT t0.U_GLO_CodeMov Folio
			FROM OVPM T0
			WHERE T0.DocType='A'
			AND T0.U_GLO_CostCenter= '{CostCenter}'
			AND T0.U_GLO_PaymentType ='GLSOV' and isnull(t0.U_GLO_CodeMov,'')<>''
			group by t0.U_GLO_CodeMov
		) B0
) T0