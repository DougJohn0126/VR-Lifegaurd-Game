using System;
using UnityEngine;


    public class TubeLauncher : MonoBehaviour
    {
        public static Action<Vector3> LaunchTubeAction;

        [SerializeField]
        private GameObject TubePrefab;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            LaunchTubeAction += LaunchTube;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LaunchTube(Vector3 destination)
        {
            Instantiate(TubePrefab, destination, new Quaternion(0, 0, 0, 0));
        }
    }

