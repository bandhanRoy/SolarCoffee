<template>
  <div v-if="isTimelineBuilt">
    <apexchart :width="'100%'" type="area" height="300" :options="options" :series="series"></apexchart>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { IInventoryTimeLine } from "../../types/InventoryGraph";
import { Sync, Get } from "vuex-pathify";
import VueApexCharts from "vue-apexcharts";

Vue.component("apexchart", VueApexCharts);

@Component({
  name: "InventoryCharts",
  components: {}
})
export default class InventoryCharts extends Vue {
  @Sync("snapshotTimeline") snapshotTimeline: IInventoryTimeLine;

  @Get("isTimelineBuilt") isTimelineBuilt?: boolean;

  get options() {
    // console.log(this.snapshotTimeline);
    return {
      dataLabels: { enabled: false },
      fill: {
        type: "gradient"
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        categories: this.snapshotTimeline.timeLine,
        type: "datetime"
      }
    };
  }

  get series() {
    return this.snapshotTimeline.productInventorySnapshots.map(snapshot => ({
      name: snapshot.productId,
      data: snapshot.quantityOnHand
    }));
  }

  async created() {
    await this.$store.dispatch("assignSnapshots");
  }
}
</script>

<style lang="scss">
</style>