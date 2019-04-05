SELECT BankCode, BankName
FROM ODSC  with (Nolock)
WHERE EXISTS (
	SELECT *
	FROM DSC1  with (Nolock)
	WHERE ODSC.BankCode = DSC1.BankCode
)