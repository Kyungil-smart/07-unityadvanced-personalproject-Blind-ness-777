# Unity Advanced Personal Project

## 프로젝트 개요
**프로젝트 명:** Battle City 3D – Grid Based Tank Game

    Unity 기반 배틀시티 스타일 3D 게임입니다.
    그리드 기반 이동 시스템과 Behavior Tree 기반 AI를 중심으로 구현했습니다.
    MVP (Minimum Viable Product) 기준으로 개발했습니다.
    5개의 고정 스테이지를 제공합니다.
    구조적 설계를 우선으로 개발을 진행했습니다.


## 프로젝트 목표(MVP)

- 5개의 고정 스테이지 구성
- Grid 기반 4방향 이동 시스템
- Behavior Tree 기반 적 AI
- 스폰 포인트 랜덤 시스템
- 승리/패배 판정 로직


## How to Play

- Unity 6000.3.9f1
- MainMenu Scene 실행
- 이동: WASD
- 발사: Mouse 1


## 프로젝트 구조

- Controller: Update 단일 관리
- Player: 이동 및 공격 처리
- Enemy: Behavior Tree 기반 의사결정
- Stage: 고정 스테이지 구성


## Git Commit Convention

Format:  
`[type]: [short description]`

Types:
- feat: 기능 추가
- fix: 버그 수정
- refactor: 구조 개선 (동작 동일)
- chore: 설정 및 기타 작업
- docs: 문서 수정

### Example

- feat: implement grid-based player movement
- fix: resolve diagonal movement issue
- refactor: separate input and movement logic
- docs: update README structure