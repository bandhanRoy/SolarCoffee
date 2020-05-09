<template>
  <div>
    <apexchart type="area" :width="'100%'" height="300" :options="options" :series="series"></apexchart>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { IInventoryTimeLine } from "../../types/InventoryGraph";
import { Sync, Get } from "vuex-pathify";
@Component({
  name: "InventoryCharts",
  components: {}
})
export default class InventoryCharts extends Vue {
  @Sync("snapshotTimeline")
  snapshotTimeline: IInventoryTimeLine;
  @Get("isTimelineBuilt")
  timelineBuilt: Boolean;

  getOptions() {
    return {
      dataLabels: { enabled: false },
      fill: {
        type: "gradient"
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        categories: this.snapshotTimeline.timeline,
        type: "datetime"
      }
    };
  }

  get series() {
    return this.snapshotTimeline.productInventorySnapshot.map(snapshot => ({
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