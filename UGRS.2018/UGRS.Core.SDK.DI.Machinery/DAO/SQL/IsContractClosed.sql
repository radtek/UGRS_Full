SELECT U_DocEntry,
	CASE 
		WHEN SUM(ISNULL(U_Close, 0)) <= 0
			THEN 0
		ELSE 1
	END AS Status
FROM [@UG_MQ_RIHR] WHERE U_DocEntry = '{ContractDocEntry}' GROUP BY U_DocEntry