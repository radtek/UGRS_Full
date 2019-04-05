SELECT BankCode, Account, Branch, GLAccount
FROM DSC1  with (Nolock)
WHERE Account = '{BankAcctNumber}'