using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.TEST
{
    public class UnitTester : MonoBehaviour
    {
        public ToolPathManagerBase TestToolPathManager;
        public RobotController TestRobotControler;

        public TCPObject RobotTCP;
        public Transform TestTCPPosition;
        public List<Transform> TestToolPathPoints;

        private void Start()
        {
            if(TestToolPathManager == null)
            {
                TestToolPathManager = FindObjectOfType<ToolPathManager>();
            }

            TestTCPMoveIKFollow(TestTCPPosition.position, TestTCPPosition.rotation);
            TestToolPathCreation();
            TestToolPathSimulation();
        }

        private void TestTCPMoveIKFollow(Vector3 pos, Quaternion rot)
        {
            RobotTCP.Set(pos, rot);
            Debug.Assert(RobotTCP.Get().position == pos," Debug Assert : TestTCPMove Failed, Position not matching");
        }

        private void TestToolPathCreation()
        {
            for(int i = 0; i< TestToolPathPoints.Count -1; i++)
            {
                TestToolPathManager.AddPointToPath(TestToolPathPoints[i]);
            }
            
        }

        private void TestToolPathSimulation()
        {
            TestRobotControler.TPSimulator.Execute();
        }
    }
}
