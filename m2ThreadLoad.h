//
/// @file m2ThreadLoad.h
/// @brief m2 ĳ���� ��׶��� �ε�(���� ĳ���� �ִ� ������, Ŭ���̾�Ʈ ����/�ִ����� ����κп��� ����� ���� ������ �߰�)
/// @date 2015-08-12
/// @author ditto
//----------------------------------------------------------------------------------------------------------------------
#pragma once

//#include <queue>

class Scene;

//
/// @brief �ε��� ������ ����
/// @date 2015-08-12
/// @author ditto
//----------------------------------------------------------------------------------------------------------------------
struct StLoadData
{
	m2::Scene* m_pkScene;			//!< ����� ������
	int m_nID;						//!< ��Ŷ ���̵�
	bool m_bComplete;				//!< �����忡�� �ε� �Ϸ� �Ǿ�����
	MHSBYTE* m_pData;				//!< ��Ŷ ������
	MHSINT32 m_nSize;				//!< ��Ŷ ������

	StLoadData()
		: m_pkScene(NULL)
		, m_nID(0)
		, m_bComplete(false)
		, m_pData(NULL)
		, m_nSize(0)
	{
	}
	virtual ~StLoadData()
	{
		if(m_pData)
			delete m_pData;
	}

};

//
/// @brief ������ ���� ť
/// @brief Push�� ��������� Pop�� ������ ó�� �Ϸ�� �Ѱ��� ������.
/// @date 2015-08-12
/// @author ditto
//----------------------------------------------------------------------------------------------------------------------
class CLoadThreadQue
{
protected:
	static std::queue<StLoadData*> m_tqLoadData;
	HANDLE m_hThread;

public:
	CLoadThreadQue();
	virtual ~CLoadThreadQue();

	void Push(m2::Scene* pkScene_, int nID_, MHSBYTE* pData_, MHSINT32 nSize_);
	static bool Pop(StLoadData** pkLoadData_o);
	bool IsEmpty();

	static unsigned __stdcall ThreadLoad(void* pTemp_);

private:
	CLoadThreadQue(CLoadThreadQue& pLoadThreadQue){};
	void operator = (CLoadThreadQue& pLoadThreadQue){};

};







