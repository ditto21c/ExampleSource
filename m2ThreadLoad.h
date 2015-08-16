//
/// @file m2ThreadLoad.h
/// @brief m2 캐릭터 백그라운드 로드(엔진 캐릭터 애니 데이터, 클라이언트 소켓/애니정보 저장부분에서 끊기는 현상 때문에 추가)
/// @date 2015-08-12
/// @author ditto
//----------------------------------------------------------------------------------------------------------------------
#pragma once

//#include <queue>

class Scene;

//
/// @brief 로드할 데이터 정보
/// @date 2015-08-12
/// @author ditto
//----------------------------------------------------------------------------------------------------------------------
struct StLoadData
{
	m2::Scene* m_pkScene;			//!< 사용할 씬정보
	int m_nID;						//!< 패킷 아이디
	bool m_bComplete;				//!< 쓰레드에서 로드 완료 되었는지
	MHSBYTE* m_pData;				//!< 패킷 데이터
	MHSINT32 m_nSize;				//!< 패킷 사이즈

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
/// @brief 쓰레드 관리 큐
/// @brief Push는 계속적으로 Pop은 기존것 처리 완료시 한개씩 빼낸다.
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







