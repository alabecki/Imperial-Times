using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System;



namespace AssemblyCsharp {
    public class NationData
    {

        public static Dictionary<int, string> uncivDict = new Dictionary<int, string>();
    public static Dictionary<int, string> minorDict = new Dictionary<int, string>();
        public static Dictionary<int, string> majorDict = new Dictionary<int, string>();

        public static Dictionary<int, string> UncivDict
        {
            get
            {
                return uncivDict;
            }
         
        }

        void Awake()
        {
            this.AddMajorNations();
            this.AddMinorNations();
            this.AddUncivNations();
        }

        public void AddUncivNations()
        {
            uncivDict.Add(4, "Aeledath");
            uncivDict.Add(11, "Alom");
            uncivDict.Add(7, "Aracher");
            uncivDict.Add(10, "Buran");
            uncivDict.Add(9, "Cath");
            uncivDict.Add(5, "Enavi");
            uncivDict.Add(8, "Isoumarai");
            uncivDict.Add(6, "Scazin");
            uncivDict.Add(3, "Se'olo");
            uncivDict.Add(0, "Veskyli");
            uncivDict.Add(1, "Xaitr");
            uncivDict.Add(2, "Zerer");


        }

        public void AddMinorNations()
        {
            minorDict.Add(20, "Dallael");
            minorDict.Add(14, "Ditaeler");
            minorDict.Add(23, "Fr'klev");
            minorDict.Add(12, "Linley");
            minorDict.Add(24, "Morlyn");
            minorDict.Add(29, "Nouphel");
            minorDict.Add(17, "Oraickoron");
            minorDict.Add(27, "Thoth");
            minorDict.Add(25, "Valden");
            minorDict.Add(13, "Wyvernmont");
            minorDict.Add(28, "Ze'ix");
            minorDict.Add(26, "Zokic");


        }

        public void AddMajorNations()
        {
            majorDict.Add(19, "Aerakrara");
            majorDict.Add(18, "An'rio");
            majorDict.Add(15, "Arozus");
            majorDict.Add(22, "Dessak");
            majorDict.Add(21, "Feandra");
            majorDict.Add(16, "Crystalice");





        }



    }

}