import { make } from "vuex-pathify";

import { IInventoryTimeLine } from "./../types/InventoryGraph";
import { InventoryService } from "./../services/inventory-service";

class GlobalStore {
    snapShotTimeline: IInventoryTimeLine = {
        productInventorySnapshot: [],
        timeline: []
    }

    isTimeLineBuildIn: boolean = false;
}

const state = new GlobalStore();

const mutations = make.mutations(state);

const actions = {
    async assignSnapshots({ commit }) {
        const inventoryService = new InventoryService();
        const res = await inventoryService.getSnapShotHistory();

        const timeline: IInventoryTimeLine = {
            productInventorySnapshot: res.productInventorySnapshot,
            timeline: res.timeline
        }

        commit('SET_SNASPSHOT_TIMELINE', timeline);
        commit('SET_IS_TIMELINE_BUILT', true);
    }
}

export default {
    state,
    mutations,
    actions,
    getters
}