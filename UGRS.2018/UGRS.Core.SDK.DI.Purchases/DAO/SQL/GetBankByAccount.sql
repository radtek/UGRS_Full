SELECT T2.Account, T1.BankName FROM ODSC T1 with (Nolock)
INNER JOIN DSC1 T2 with (Nolock)
	ON T1.BankCode = T2.BankCode
WHERE T2.Account = '{AccountNumber}'