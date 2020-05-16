import { mount } from "@vue/test-utils";
import SolarButton from "@/components/SolarButton.vue";

describe('SideMenu.vue', () => {
    it("it displays text in default slot position", () => {
        const wrapper = mount(SolarButton, {
            propsData: {},
            slots: {
                default: "click here!"
            }
        });
        expect(wrapper.find("button").text()).toEqual("click here!");
    });

    it("has underlying button disabled button if disabled passed as props", () => {
        const wrapper = mount(SolarButton, {
            propsData: {
                disabled: true
            },
            slots: {
                default: "foo"
            }
        });
        expect(wrapper.find('input:disabled'));
    });

    it("has no underlying button disabled button if disabled passed as false as props", () => {
        const wrapper = mount(SolarButton, {
            propsData: {
                disabled: false
            },
            slots: {
                default: "foo"
            }
        });
        expect(!wrapper.find('input:disabled'));
    });
});

