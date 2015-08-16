#include "stdafxM2.h"

//#include "m2ThreadLoad.h"

std::queue<StLoadData*> CLoadThreadQue::m_tqLoadData = std::queue<StLoadData*>();

CLoadThreadQue::CLoadThreadQue()
{
	m_hThread = (HANDLE)_beginthreadex(NULL, 0, CLoadThreadQue::ThreadLoad, NULL, NULL, NULL);
}

CLoadThreadQue::~CLoadThreadQue()
{
	_endthreadex(0);
}

void CLoadThreadQue::Push(m2::Scene* pkScene_, int nID_, MHSBYTE* pData_, MHSINT32 nSize_)
{
	StLoadData* pkLoadData = new StLoadData();
	pkLoadData->m_pkScene = pkScene_;
	pkLoadData->m_nID = nID_;
	pkLoadData->m_pData = pData_;
	pkLoadData->m_nSize = nSize_;
	m_tqLoadData.push(pkLoadData);
}

bool CLoadThreadQue::Pop(StLoadData** ppkLoadData_o)
{
	if(m_tqLoadData.empty())
		return false;
	
	StLoadData* pkLoadData = m_tqLoadData.front();
	if(!pkLoadData->m_bComplete)
	{
		*ppkLoadData_o = pkLoadData;
		return true;
	}
	
	delete pkLoadData;
	m_tqLoadData.pop();

	return false;
}

bool CLoadThreadQue::IsEmpty()
{
	return m_tqLoadData.empty();
}

unsigned __stdcall CLoadThreadQue::ThreadLoad(void* pTemp_)
{
	StLoadData* pLoadData = NULL;
	while(1)
	{
		if(true == Pop(&pLoadData))
		{
			if((int)m2::CPID_FIELD_IN_CHARACTER_INFO == pLoadData->m_nID)
			{
				((m2::SceneGame*)pLoadData->m_pkScene)->FieldInCharacterInfo(pLoadData->m_pData, pLoadData->m_nSize);
			}
			else if((int)m2::CPID_FIELD_OUT_CHARACTER_INFO == pLoadData->m_nID)
			{
				((m2::SceneGame*)pLoadData->m_pkScene)->FieldOutCharacterInfo(pLoadData->m_pData, pLoadData->m_nSize);
			}
			else if((int)m2::CPID_FIELD_WARP_CHARACTER == pLoadData->m_nID)
			{
				((m2::SceneGame*)pLoadData->m_pkScene)->FieldWarpCharacter(pLoadData->m_pData, pLoadData->m_nSize);
			}

			pLoadData->m_bComplete = true;
		}
	}
	return 0;
}