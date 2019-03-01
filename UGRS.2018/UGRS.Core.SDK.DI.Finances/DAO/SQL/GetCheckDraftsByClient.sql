SELECT T1.DocEntry AS DraftDocEntry, DueDate, CheckNum, T0.BankCode, T0.CheckSum, Currency, CheckAct, DocDate, CardCode
FROM PDF1 T0
LEFT JOIN OPDF T1 ON (T1.DocEntry = T0.DocNum)
WHERE T1.DocType = 'C' AND T1.Canceled = 'N' AND T1.CardCode = '{CardCode}'