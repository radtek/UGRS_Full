SELECT *FROM [@UG_TR_CMSN] A1 with (nolock)
WHERE ISNULL(U_HasDriverCms, 'N') = 'N'
	  AND ISNULL(U_AutTrans, 'N') = 'Y'
	  AND ISNULL(U_AutOperations, 'N') = 'Y'
	  AND ISNULL(U_AutBanks, 'N') = 'Y'