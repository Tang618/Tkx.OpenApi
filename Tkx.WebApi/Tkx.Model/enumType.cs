namespace Tkx.Model
{
    /*全局定义点 */
    #region 暂未使用
    ///// <summary>支付类型    
    /////0：现金      
    //////1：自助缴费终端
    /////2：手持终端
    /////3：中心缴费
    //////4：停开心余额         
    /////5：微信         
    //////6：支付宝
    /////7：银联 
    /////8：抵扣
    /////9：其他   
    ///// </summary>
    //public enum Enum_payMethod
    //{
    //    现金 = 0,
    //    自助缴费终端 = 1,
    //    手持终端 = 2,
    //    中心缴费 = 3,
    //    停开心余额 = 4,
    //    微信 = 5,
    //    支付宝 = 6,
    //    银联 = 7,
    //    抵扣8,
    //    其他 = 9
    //}

    ///// <summary> 支付中心要使用的
    ///// </summary>
    //public enum Enum_PaymentStatus
    //{
    //    待支付 = 0,
    //    支付 = 1,
    //    支付完成 = 2,
    //    退款 = 3

    //}

    ///// <summary>停车类型总分类 
    ///// </summary>
    //public enum Enum_ParKingCarType
    //{
    //    临停,
    //    储值,
    //    储次, 
    //    月卡,
    //    季卡,
    //    半年卡,
    //    年卡,

    //}
    #endregion



    /// <summary>支付渠道payChannel
    /// 
    /// </summary>
    public enum Enum_payChannel
    {
        现金 = 0,
        微信 = 1,
        支付宝 = 2,
        银联 = 3,
        自助缴费终端 = 4,
        手持终端 = 5,
        中心缴费 = 6,
        停开心余额 = 7,
        抵扣 = 8,
        其他 = 9 
    }
    /// <summary>
    /// 订单状态OrderState
    /// </summary>
    public enum Enum_OrderState
    {
        成功 = 0,
        失败 = 1,
        未知 = 2,
        已撤销 = 3,
        已关闭 = 4,
        有退货 = 5,
        其他 = 6
    }


    /// <summary>交易类型TradingType
    /// 
    /// </summary>
    public enum Enum_TradingType
    {
        现金 = 0,
        消费 = 1,
        撤销 = 2,
        退款 = 3,
        充值 = 4,
        圈存 = 5,
        其他 = 6
    }




    /// <summary>进场车辆类型
    /// 进场车辆类型使用
    /// </summary>
    public enum Enum_CarType
    {
        临时车 = 0, 
        临时授权车 = 1,
        预约车 = 2,
        月租车 = 3,
        VIP车 = 4,
        特殊车 = 5,
        储值车 = 6,
        储时车 = 7,
        储次车 = 8,
        白名单车 = 9,
        黑名单车 = 10 
    }

    /// <summary>车牌颜色区分用于后期计费
    /// 进场车辆类型使用
    /// </summary>
    public enum Enum_CarNoColoType
    {
        蓝牌 = 0,
        黄牌 = 1
    }
    /// <summary>车辆进场类型
    /// 
    /// </summary>
    public enum Enum_enterParkType
    {
        车牌识别自动开闸 = 0,
        手动开闸 = 1,
        遥控开闸 = 2,
        地感开闸 = 3

    }
}
