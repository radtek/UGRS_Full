SELECT BankCode, Account, Branch, GLAccount
FROM DSC1 with (nolock)
WHERE BankCode = '{BankCode}'