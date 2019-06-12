﻿using System;
using System.Collections.Generic;
using Common;

namespace Core
{
    using AuraApplicationList = List<AuraApplication>;
    using AuraEffectList = List<AuraEffect>;
    using AuraApplicationMap = Dictionary<Guid, AuraApplication>;
    using SpellEffectInfoList = List<SpellEffectInfo>;

    public class Aura
    {
        private int maxDuration;
        private bool isSingleTarget;

        private AuraApplicationMap applications = new AuraApplicationMap();
        private AuraEffectList effects = new AuraEffectList();
        private SpellEffectInfoList spelEffectInfos = new SpellEffectInfoList();

        public SpellInfo SpellInfo { get; private set; }
        public ulong CastId { get; private set; }
        public ulong CasterId { get; private set; }
        public int SpellVisualId { get; private set; }

        public int Id => SpellInfo.Id;
        public Unit Caster => null;
        public Unit Owner { get; private set; }


        public Aura(SpellInfo spellproto, ulong castId, Unit owner, Unit caster)
        {

        }

        #region Aura types

        public bool HasMoreThanOneEffectForType(AuraType auraType)
        {
            return false;
        }

        public bool IsArea()
        {
            return false;
        }

        public bool IsPassive()
        {
            return false;
        }

        public bool IsRemoved()
        {
            return false;
        }

        public bool IsSingleTarget()
        {
            return isSingleTarget;
        }

        public bool IsSingleTargetWith(Aura aura)
        {
            return false;
        }

        public void SetIsSingleTarget(bool val)
        {
            isSingleTarget = val;
        }

        public void UnregisterSingleTarget()
        {
        }

        public int CalcDispelChance(Unit auraTarget, bool offensive)
        {
            return 0;
        }

        //List<AuraScript> m_loadedScripts;

        public AuraEffectList GetAuraEffects() { return effects; }

        public SpellEffectInfoList GetSpellEffectInfos() { return spelEffectInfos; }

        public SpellEffectInfo GetSpellEffectInfo(int index)
        {
            return null;
        }

        #endregion

        #region Application, updates and removal

        public static Aura TryRefreshStackOrCreate(SpellInfo spellProto, Unit owner, ref bool refresh, List<int> baseAmount, ulong castId, ulong targetCasterId, ulong originalCasterId)
        {
            Assert.IsNotNull(spellProto);
            Assert.IsNotNull(owner);
            Assert.IsTrue(originalCasterId != 0 || targetCasterId != 0);

            if (refresh)
                refresh = false;

            var foundAura = owner.TryStackingOrRefreshingExistingAura(spellProto, originalCasterId, targetCasterId, baseAmount);
            if (foundAura == null)
                return Create(spellProto, owner, baseAmount, castId, originalCasterId, targetCasterId);

            if (foundAura.IsRemoved())
                return null;

            refresh = true;
            return foundAura;
        }

        public static Aura Create(SpellInfo spellProto, Unit owner, List<int> baseAmount, ulong castId, ulong originalCasterId, ulong targetCasterId)
        {
            Assert.IsNotNull(spellProto);
            Assert.IsNotNull(owner);
            Assert.IsTrue(originalCasterId != 0 || targetCasterId != 0);

            if (originalCasterId == 0)
                originalCasterId = targetCasterId;

            Unit caster = owner.NetworkId == originalCasterId ? owner : owner.Map.FindMapEntity<Unit>(originalCasterId);
            Aura aura = new UnitAura(spellProto, owner, caster, baseAmount, castId);
            return aura.IsRemoved() ? null : aura;
        }


        public void InitEffects(Unit caster, List<int> baseAmount)
        {

        }

        public virtual void ApplyForTarget(Unit target, Unit caster, AuraApplication auraApp)
        {
        }

        public virtual void UnapplyForTarget(Unit target, Unit caster, AuraApplication auraApp)
        {
        }

        public virtual void Remove(AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
        }

        public virtual void FillTargetMap(Dictionary<Unit, uint> targets, Unit caster)
        {
        }

        public void UpdateTargetMap(Unit caster, bool apply)
        {

        }

        public void RegisterForTargets()
        {
            UpdateTargetMap(Caster, false);
        }

        public void ApplyForTargets()
        {
            UpdateTargetMap(Caster, true);
        }

        public void ApplyEffectForTargets(EffectTeleportDirect effect)
        {
        }

        public void UpdateOwner(uint diff, Unit owner)
        {
        }

        public void Update(uint diff, Unit caster)
        {
        }

        public void RefreshSpellMods()
        {

        }

        private void _DeleteRemovedApplications()
        {
        }

        #endregion

        #region Time and duration

        public int GetMaxDuration()
        {
            return maxDuration;
        }

        public void SetMaxDuration(int duration)
        {
            maxDuration = duration;
        }

        public int CalcMaxDuration()
        {
            return CalcMaxDuration(Caster);
        }

        public int CalcMaxDuration(Unit caster)
        {
            return 0;
        }

        public void SetDuration(int duration, bool withMods = false)
        {
        }

        public void RefreshDuration(bool withMods = false)
        {
        }

        public void RefreshTimers()
        {
        }

        public bool IsPermanent()
        {
            return GetMaxDuration() == -1;
        }

        #endregion

        #region Charges and stacks

        public void SetCharges(ushort charges)
        {
        }

        public ushort CalcMaxCharges(Unit caster)
        {
            return 0;
        }

        public ushort CalcMaxCharges()
        {
            return CalcMaxCharges(Caster);
        }

        public bool ModCharges(int num, AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
            return true;
        }

        public bool DropCharge(AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
            return ModCharges(-1, removeMode);
        }

        public void ModChargesDelayed(int num, AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
        }

        public void DropChargeDelayed(uint delay, AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
        }

        public void SetStackAmount(uint num)
        {
        }

        public bool ModStackAmount(int num, AuraRemoveMode removeMode = AuraRemoveMode.Default)
        {
            return false;
        }

        #endregion

        #region Aura effect helpers

        public bool HasEffect(ushort effIndex)
        {
            return GetEffect(effIndex) != null;
        }

        public bool HasEffectType(AuraType type)
        {
            return false;
        }

        public AuraEffect GetEffect(int index)
        {
            return null;
        }

        public uint GetEffectMask()
        {
            return 0;
        }

        public void RecalculateAmountOfEffects()
        {
        }

        public void HandleAllEffects(AuraApplication aurApp, ushort mode, bool apply)
        {
        }

        #endregion

        #region Target helpers

        public AuraApplicationMap GetApplicationMap()
        {
            return applications;
        }

        public void GetApplicationList(AuraApplicationList applicationList)
        {
        }

        public AuraApplication GetApplicationOfTarget(Guid guid)
        {
            AuraApplication targetApplication;
            applications.TryGetValue(guid, out targetApplication);
            return targetApplication;
        }

        public bool IsAppliedOnTarget(Guid guid)
        {
            return applications.ContainsKey(guid);
        }

        public void SetNeedClientUpdateForTargets()
        {
            
        }

        public void HandleAuraSpecificMods(AuraApplication aurApp, Unit caster, bool apply, bool onReapply)
        {
            
        }

        public void HandleAuraSpecificPeriodics(AuraApplication aurApp, Unit caster)
        {
            
        }

        public bool CanBeAppliedOn(Unit target)
        {
            return false;
        }

        public bool CheckAreaTarget(Unit target)
        {
            return false;
        }

        public bool CanStackWith(Aura existingAura)
        {
            return false;
        }

        #endregion

        #region Aura scripts

        public void LoadScripts()
        {
        }

        public bool CallScriptCheckAreaTargetHandlers(Unit target)
        {
            return false;
        }

        public void CallScriptDispel(SpellDispelInfo spellDispelInfo)
        {
        }

        public void CallScriptAfterDispel(SpellDispelInfo spellDispelInfo)
        {
        }

        public bool CallScriptEffectApplyHandlers(AuraEffect aurEff, AuraApplication aurApp, AuraEffectHandleMode mode)
        {
            return false;
        }

        public bool CallScriptEffectRemoveHandlers(AuraEffect aurEff, AuraApplication aurApp, AuraEffectHandleMode mode)
        {
            return false;
        }

        public void CallScriptAfterEffectApplyHandlers(AuraEffect aurEff, AuraApplication aurApp, AuraEffectHandleMode mode)
        {
        }

        public void CallScriptAfterEffectRemoveHandlers(AuraEffect aurEff, AuraApplication aurApp, AuraEffectHandleMode mode)
        {
        }

        public bool CallScriptEffectPeriodicHandlers(AuraEffect aurEff, AuraApplication aurApp)
        {
            return false;
        }

        public void CallScriptEffectUpdatePeriodicHandlers(AuraEffect aurEff)
        {
            
        }

        public void CallScriptEffectCalcAmountHandlers(AuraEffect aurEff, ref int amount, ref bool canBeRecalculated)
        {
            
        }

        public void CallScriptEffectCalcPeriodicHandlers(AuraEffect aurEff, ref bool isPeriodic, ref int amplitude)
        {
            
        }

        public void CallScriptEffectCalcSpellModHandlers(AuraEffect aurEff, SpellModifier spellMod)
        {
            
        }

        public void CallScriptEffectAbsorbHandlers(AuraEffect aurEff, AuraApplication aurApp, SpellDamageInfo dmgInfo, uint absorbAmount, bool defaultPrevented)
        {
            
        }

        public void CallScriptEffectAfterAbsorbHandlers(AuraEffect aurEff, AuraApplication aurApp, SpellDamageInfo dmgInfo, uint absorbAmount)
        {
            
        }

        #endregion
    }
}