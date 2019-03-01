SELECT 
	isnull(SUM(T0.DocTotal - T0.PaidToDate),0)   [DebtImport]
FROM 
	OINV T0
WHERE 
	T0.CANCELED = 'N' AND T0.DocStatus='O' AND T0.Series = '{Series}' and T0.CardCode='{CardCode}'

