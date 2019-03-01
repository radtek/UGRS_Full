SELECT T0.BankCode, T1.BankName
FROM OCRB T0
LEFT JOIN ODSC T1 ON (T1.BankCode = T0.BankCode)
WHERE CardCode = '{CardCode}'
GROUP BY T0.BankCode, T1.BankName
