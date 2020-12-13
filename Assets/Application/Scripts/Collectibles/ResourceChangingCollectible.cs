using LabirinthGame.Effects;
using LabirinthGame.Stats;
using UnityEngine;

namespace LabirinthGame.Collectibles
{
    public class ResourceChangingCollectible : CollectibleBase
    {
        [SerializeField] private CharacterResourceType type = CharacterResourceType.Undefined;
        [SerializeField] private float amount = 50f;
        [SerializeField] private float duration = 5f;
        [SerializeField] private bool changeRegeneration;
        protected override void OnCollectEffect(Collider other)
        {
            if (other.TryGetComponent(out IEffectApplicable target))
            {
                EffectBase effect;
                if (changeRegeneration)
                    effect = new ResourceRegenerationChangingEffect(target, type, amount, duration, duration>0 ? EffectDuration.Timed : EffectDuration.Permanent, amount>0 ? EffectType.Positive : EffectType.Negative);
                else
                effect = new ResourceValueChangingEffect(target, type, amount, duration, duration>0 ? EffectDuration.Timed : EffectDuration.Permanent, amount>0 ? EffectType.Positive : EffectType.Negative);

                target.ApplyEffect(effect);
                Destroy(gameObject);
            }
        }
    }
}