using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel
{
    public class Grip
    {
        public Grip(double _xdistn1,double _ydistn1,double _xdistn2, double _ydistn2 , double _angle)
        {
            xdistn1 = _xdistn1;
            ydistn1 = _ydistn1;
            xdistn2 = _xdistn2;
            ydistn2 = _ydistn2;
            angle = _angle;
        }
        public double xdistn1 { get; set; }
        public double ydistn1 { get; set; }
        public double xdistn2 { get; set; }
        public double ydistn2 { get; set; }
        public double angle { get; set; }
    }

    public class FeatureGripsMapping
    {
        public Dictionary<string, Grip> grips = new Dictionary<string, Grip>();

        public static FeatureGripsMapping instance = new FeatureGripsMapping();

        public FeatureGripsMapping()
        {
            //grips.Add("40", "电流互感器");
            //grips.Add("40", new Grip(0, 0, -0, -0, 0));
            //grips.Add("71", "穿墙套管");
            //grips.Add("71", new Grip(0, -0, -0, -0, 0));
            //grips.Add("72", "接地挂环");
            //grips.Add("72", new Grip(0, -0, -0, -0, 0));
            //grips.Add("73", "低压断连");
            grips.Add("73", new Grip(0.00000651, -0.00000157, -0.00000651, -0.00000157, 0));
            //grips.Add("74", "低压终端");
            grips.Add("74_0", new Grip(0.00000289, -0, -0, -0, 0));
            grips.Add("74_1", new Grip(0.00000377, 0.00000001, -0.00000387, -0.00000005, 0));
            //grips.Add("76", "关口计量柜");
            //grips.Add("76", new Grip(0, -0, -0, -0, 0));
            //grips.Add("84", "计量柜");
            grips.Add("84_0", new Grip(-0.00000027, 0.00000572, -0.00000027, -0.00000621, 0));
            grips.Add("84_1", new Grip(-0.00000010, 0.00000582, -0.00000010, -0.00000639, 0));
            //grips.Add("85", "PT柜");
            grips.Add("85", new Grip(0.00000216, 0.00000279, -0, -0, 0));
            //grips.Add("146", "10kV开关");
            grips.Add("146_0_0", new Grip(0.00000308, -0.00000009, -0.00000358, -0.00000009, 0));
            grips.Add("146_0_1", new Grip(0.00000302, -0.00000005, -0.00000355, -0.00000005, 0));
            grips.Add("146_1_0", new Grip(0.00000355, 0.00000006, -0.00000340, 0.00000006, 0));
            grips.Add("146_1_1", new Grip(0.00000355, 0.00000006, -0.00000340, 0.00000006, 0));
            grips.Add("146_2_0", new Grip(0.00000493, -0.00000041, -0.00000167, -0.00000041, 0));
            grips.Add("146_2_1", new Grip(0.00000460, -0.00000048, -0.00000203, -0.00000048, 0));
            grips.Add("146_3_0", new Grip(0.00000348, 0, -0.00000354, 0, 0));
            grips.Add("146_3_1", new Grip(0.00000351, 0, -0.00000355, 0, 0));
            grips.Add("146_4_0", new Grip(0.00000326, 0, -0.00000363, 0, 0));
            grips.Add("146_4_1", new Grip(0.00000326, 0, -0.00000363, 0, 0));
            grips.Add("146_5_0", new Grip(0.00000326, 0, -0.00000338, 0, 0));
            grips.Add("146_5_1", new Grip(0.00000326, 0, -0.00000338, 0, 0));
            grips.Add("146_6", new Grip(0.00000400, 0, -0.00000287, 0, 0));
            grips.Add("146_7", new Grip(0.00000322, 0, -0.00000322, 0, 0));
            grips.Add("146_8_0", new Grip(0.00000801, -0.00000010, -0.00000513, -0.00000010, 0));
            grips.Add("146_8_1", new Grip(0.00000752, -0.00000019, -0.00000555, -0.00000017, 0));
            grips.Add("146_9_0", new Grip(0.00000329, 0, -0.00000367, 0, 0));
            grips.Add("146_9_1", new Grip(0.00000329, 0, -0.00000367, 0, 0));
            grips.Add("146_10_0", new Grip(0.00000316, -0.00000010, -0.00000366, -0.00000010, 0));
            grips.Add("146_10_1", new Grip(0.00000310, -0.00000006, -0.00000367, -0.00000006, 0));
            //grips.Add("147", "站房电缆头");
            grips.Add("147_0", new Grip(0.00000168, 0, -0.00000192, 0, 0));
            grips.Add("147_1", new Grip(-0.00000240, 0, 0, 0, 0));
            //features.Add("148", "变压器");
            grips.Add("148_0", new Grip(0, 0.00000634, 0, -0.00000635, 0));
            grips.Add("148_1", new Grip(0, 0.00000631, 0, -0.00000630, 0));
            grips.Add("148_2", new Grip(0, 0.00000635, 0, -0.00000630, 0));
            grips.Add("148_3", new Grip(0, 0.00000631, 0, -0.00000635, 0));
            grips.Add("148_4", new Grip(0, 0.00000613, 0, -0.00000600, 0));
            //features.Add("150", "10kV电缆中间接头");
            //grips.Add("150", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("151", "10kV电缆终端头");
            grips.Add("151_0", new Grip(0.00000184, 0, -0.00000192, 0, 0));
            grips.Add("151_1", new Grip(-0.00000244, 0, -0, 0, 0));
            //grips.Add("152", "断路器");
            grips.Add("152_0", new Grip(0.00000309, -0.00000009, -0.00000354, -0.00000009, 0));
            grips.Add("152_1", new Grip(0.00000303, -0.00000005, -0.00000357, -0.00000005, 0));
            //grips.Add("155", "低压开关");
            grips.Add("155_0_0", new Grip(0.00000223, 0.00000013, -0.00000221, 0.00000013, 0));
            grips.Add("155_0_1", new Grip(0.00000223, 0.00000013, -0.00000221, 0.00000013, 0));
            grips.Add("155_1_0", new Grip(0.00000349, 0.00000013, -0.00000357, 0.00000013, 0));
            grips.Add("155_1_1", new Grip(0.00000349, 0.00000013, -0.00000357, 0.00000013, 0));
            //grips.Add("158", "低压电缆中间接头");
            //grips.Add("158", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("159", "低压集中抄表箱");
            //grips.Add("159_0", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("159_1", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("159_2", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("169", "低压电缆头");
            grips.Add("169_0", new Grip(0.00000179, -0, -0.00000161, -0, 0));
            grips.Add("169_1", new Grip(-0.00000230, -0, -0, -0, 0));
            //grips.Add("171", "用户自带发电机");
            //grips.Add("171", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("172", "电动机");
            //grips.Add("172", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("173", "站房接地刀闸");
            grips.Add("173_0_0", new Grip(-0.00000140, -0.00000208, -0, -0, 0));
            grips.Add("173_0_1", new Grip(-0.00000156, -0.00000208, -0, -0, 0));   
            //grips.Add("173_1_0", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("173_1_1", new Grip(-0, -0, -0, -0, 0));   
            //grips.Add("174", "无功补偿器");
            //grips.Add("175", "10kV断连");
            grips.Add("175", new Grip(0.00000646, -0.00000156, -0.00000646, -0.00000156, 0));
            //grips.Add("176", "解断口");
            grips.Add("176", new Grip(0.00000194, -0, -0.00000194, -0, 0));
            //grips.Add("177", "避雷器");
            grips.Add("177_1", new Grip(0.00000775, 0.00000006, 0, -0, 0));
            grips.Add("177_0", new Grip(0.00000411, -0.00000332, -0, -0, 0));
            //grips.Add("188", "负控");
            //grips.Add("188", new Grip(-0, -0, -0, -0, 0));
            //grips.Add("307", "电压互感器");
            //grips.Add("307", new Grip(0, 0.00000206, -0, -0, 0));
            //grips.Add("309", "高压电机");
            //grips.Add("309", new Grip(0, 0, -0, -0, 0));
            //grips.Add("320", "监视装置");
            //grips.Add("320", new Grip(0, 0, -0, -0, 0));
        }
    }
}
