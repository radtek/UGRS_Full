SELECT T1.CodeMov, T1.FolioOri, T2.U_Status FROM
(
	SELECT MAX(U_GLO_CodeMov) CodeMov, MAX(U_GLO_FolioOri) FolioOri 
	FROM OVPM with (nolock) 
	WHERE U_GLO_PaymentType = 'GLSOV' AND U_GLO_CostCenter = 'MQ_MAQUI' AND Canceled = 'N' AND U_GLO_FolioOri = '{FolioRise}'
) T1
INNER JOIN
(
	SELECT U_CodeMov, U_Status FROM [@UG_GLO_VOUC] with (nolock)  WHERE U_CodeMov = 'CV_MQ_MAQUI_7'
) T2 ON T2.U_CodeMov = T1.CodeMov