﻿using UnityEngine;

namespace AYS.Server {
	[CreateAssetMenu]
	public class ServerConfiguration : ScriptableObject {
		public int port = 8080;
		public string ipAddr = "";
	}
}

