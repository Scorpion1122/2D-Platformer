using System.Collections;
using UnityEngine;

namespace Thijs.Platformer.Gimmicks
{
    public class Bomb : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private float explosionDelay = 1f;
        [SerializeField] private new ParticleSystem particleSystem;

        private Coroutine fuseCoroutine;

        private void Awake()
        {
            particleSystem.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopCoroutines();
        }
        
        public void GetDamaged(Damage damage)
        {
            if (fuseCoroutine != null)
                return;
            
            if (damage.type == DamageType.Bludgeoning)
                IgniteFuse();
            else if (damage.type == DamageType.Explosion)
                Explode();
        }

        public void IgniteFuse()
        {
            if (fuseCoroutine != null)
                return;

            fuseCoroutine = StartCoroutine(FuseEnumerator());
        }

        private IEnumerator FuseEnumerator()
        {
            particleSystem.gameObject.SetActive(true);
            yield return new WaitForSeconds(explosionDelay);
            particleSystem.gameObject.SetActive(false);
            Explode();
        }

        private void Explode()
        {
            StopCoroutines();
            
            //Physics and Damage
        }

        private void StopCoroutines()
        {
            if (fuseCoroutine != null)
                StopCoroutine(fuseCoroutine);
            fuseCoroutine = null;
        }
    }
}