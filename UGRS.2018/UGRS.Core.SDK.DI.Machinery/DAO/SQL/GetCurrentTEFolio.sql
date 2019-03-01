SELECT U_GLO_CodeMov AS Folio
FROM OVPM
WHERE Canceled = 'N' AND U_GLO_FolioOri = '{RiseId}' AND U_GLO_ObjType = 'frmTExp'