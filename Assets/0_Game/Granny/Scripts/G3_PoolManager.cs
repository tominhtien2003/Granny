using UnityEngine;
using System.Collections.Generic;

public class G3_PoolManager : PoolManager
{
    public static G3_PoolManager Instance;

    public PoolItem<G3_Bullet> bulletPool;
    public PoolItem<G3_BotHealth> botPool;


    void Awake()
    {
        Instance = this;
        InitPool(bulletPool);
        InitPool(botPool);
    }
    public G3_Bullet GetBullet()
    {
        return Get(bulletPool);
    }
    public G3_BotHealth GetBot()
    {
        return Get(botPool);
    }
    public void ReturnBullet(G3_Bullet bullet)
    {
        Return(bulletPool, bullet);
    }
    public void ReturnBot(G3_BotHealth bot)
    {
        Return(botPool, bot);
    }
}
