namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Unity.Map;
	using UnityEngine;

	public class ImmediatePositionWithLocationProvider : MonoBehaviour
	{
        //[SerializeField]
        //private UnifiedMap _map;

        //Editing By Scotty
        private Vector3 oldPosition;
        private float speed = 2.0f;

		bool _isInitialized;

		ILocationProvider _locationProvider;
		ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}

		Vector3 _targetPosition;

		void Start()
		{
			LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
		}

		void LateUpdate()
		{
			if (_isInitialized)
			{
				var map = LocationProviderFactory.Instance.mapManager;
                oldPosition = this.transform.localPosition;

                float lerpspeed = speed * Time.deltaTime;

                Vector3 newPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);

                transform.localPosition = Vector3.Lerp(oldPosition, newPosition, lerpspeed);

				//transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude); Too Jerky - Scott
			}
		}
	}
}