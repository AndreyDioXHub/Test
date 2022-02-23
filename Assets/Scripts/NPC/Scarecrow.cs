using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scarecrow : MonoBehaviour
{
    [SerializeField]
    private Image _hpBar;
    
    [SerializeField]
    private List<MeshRenderer> _bodyMaterials = new List<MeshRenderer>();
    [SerializeField]
    private float _hpMax = 1000;
    [SerializeField]
    private float _hpCur;

    [SerializeField]
    private int _wetnessMax = 100;
    private int _wetnessCur;

    private float _burnedTimeMax;
    private float _burnedTimeCur;
    private bool _itBurn;

    [SerializeField]
    private Debuff _curDebuff;

    // Start is called before the first frame update
    void Start()
    {
        Recover();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewDebuff(Debuff debuff)
    {
        if (_wetnessCur == 0)
        {
            if (debuff.GetDebuffType().DType == TypeDebuff.Fire)
            {
                _curDebuff = debuff;

                if(_itBurn == true)
                {
                    _burnedTimeCur = 0;
                }
                else
                {
                    _burnedTimeCur = 0;
                    _burnedTimeMax = debuff.GetDebuffType().Duration;
                    _itBurn = true;
                    StartCoroutine(Burn(debuff.GetDebuffType().Period, debuff.GetDebuffType().Misc));
                }
                
            }
            else if (debuff.GetDebuffType().DType == TypeDebuff.Water)
            {
                _curDebuff = debuff;
                _wetnessCur += (int)debuff.GetDebuffType().Misc;
            }
        }  
        else if (_wetnessCur > 0 && _wetnessCur <= _wetnessMax)
        {
            if (debuff.GetDebuffType().DType == TypeDebuff.Fire)
            {
                _wetnessCur -= 1;// (int)debuff.GetDebuffType().Misc;
            }
            else if (debuff.GetDebuffType().DType == TypeDebuff.Water)
            {
                _curDebuff = debuff;
                _wetnessCur += (int)debuff.GetDebuffType().Misc;
            }

            if (_wetnessCur <= 0)
            {
                _wetnessCur = 0;
                _curDebuff = new NormalState();
            }
            else if(_wetnessCur >= _wetnessMax)
            {
                _wetnessCur = _wetnessMax;
            }
        }
        else if (_wetnessCur >= _wetnessMax)
        {
            _wetnessCur = _wetnessMax;
            if (debuff.GetDebuffType().DType == TypeDebuff.Fire)
            {
                _wetnessCur -= (int)debuff.GetDebuffType().Misc;
            }
            else if (debuff.GetDebuffType().DType == TypeDebuff.Water)
            {
                _curDebuff = debuff;
            }
        }
        else
        {
            Debug.Log("хз как тут оказались");
        }

        foreach (var m in _bodyMaterials)
        {
            m.material.SetColor("_Color", _curDebuff.GetDebuffType().HPBarColor);
        }
        _hpBar.color = _curDebuff.GetDebuffType().HPBarColor;
    }

    IEnumerator Burn(float period, float damage)
    {
        while(_burnedTimeCur < _burnedTimeMax)
        {
            yield return new WaitForSeconds(period);
            _burnedTimeCur += period;
            _hpCur -= damage;
            _hpBar.fillAmount = _hpCur / _hpMax;
            CheckDeath();
        }
        _itBurn = false;
    }

    public void SetDamage(float damage)
    {
        _hpCur = _hpCur - damage - _curDebuff.HpDebuff();
        _hpBar.fillAmount = _hpCur / _hpMax;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_hpCur < 0)
        {
            Recover();
        }
    }

    public void Recover()
    {
        _curDebuff = new NormalState();

        foreach (var m in _bodyMaterials)
        {
            m.material.SetColor("_Color", _curDebuff.GetDebuffType().HPBarColor);
        }

        _hpBar.color = _curDebuff.GetDebuffType().HPBarColor;

        _hpCur = _hpMax;
        _hpBar.fillAmount = _hpCur / _hpMax;

        _wetnessCur = 0;
        _burnedTimeMax = 0;
        _burnedTimeCur = _burnedTimeMax;
    }
}
