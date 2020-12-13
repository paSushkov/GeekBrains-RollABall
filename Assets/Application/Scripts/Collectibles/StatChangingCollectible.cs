using LabirinthGame.Effects;
using LabirinthGame.Stats;
using UnityEngine;

namespace LabirinthGame.Collectibles
{
    public class StatChangingCollectible : CollectibleBase
    {
        [SerializeField] private CharacteristicType type = CharacteristicType.Undefined;
        [SerializeField] private float amount = 50f;
        [SerializeField] private float duration = 5f;

        protected override void OnCollectEffect(Collider other)
        {
            if (other.TryGetComponent(out IEffectApplicable target))
            {
                var effect = new CharacteristicChangingEffect(target, type, amount, duration, duration>0 ? EffectDuration.Timed : EffectDuration.Permanent, amount>0 ? EffectType.Positive : EffectType.Negative);
                target.ApplyEffect(effect);
                Destroy(gameObject);
            }
        }
    }
}