﻿namespace Serenity {
    function vueIntegration() {
        // @ts-ignore
        if (typeof Vue === "undefined")
            return false; 
        // @ts-ignore
        Vue.component('editor', {
            props: {
                type: {
                    type: String,
                    required: true,                    
                },
                id: {
                    type: String,
                    required: false
                },
                name: {
                    type: String,
                    required: false
                },
                placeholder: {
                    type: String,
                    required: false
                },
                value: {
                    required: false
                },
                options: {
                    required: false
                },
                maxLength: {
                    required: false
                }
            },
            render: function (createElement: any) {
                var editorType = Serenity.EditorTypeRegistry.get(this.type);
                var elementAttr = Q.getAttributes(editorType, Serenity.ElementAttribute, true);
                var elementHtml = ((elementAttr.length > 0) ? elementAttr[0].value : '<input/>') as string;
                var domProps: any = {};
                var element = $(elementHtml)[0];
                var attrs = element.attributes;
                for (var i = 0; i < attrs.length; i++) {
                    var attr = attrs.item(i);
                    domProps[attr.name] = attr.value;
                }

                if (this.id != null)
                    domProps.id = this.id;

                if (this.name != null)
                    domProps.name = this.name;

                if (this.placeholder != null)
                    domProps.placeholder = this.placeholder;

                var editorParams = this.options;
                var optionsType: any = null;

                var self = this
                var el = createElement(element.tagName, {
                    domProps: domProps
                });

                this.$editorType = editorType;

                return el;
            },
            watch: {
                value: function (v: any) {
                    Serenity.EditorUtils.setValue(this.$widget, v);
                }
            },
            mounted: function () {
                var self = this;

                this.$widget = new this.$editorType($(this.$el), this.options);
                this.$widget.initialize();

                if (this.maxLength) {
                    (Serenity.PropertyGrid as any).$setMaxLength(this.$widget, this.maxLength);
                }

                if (this.options)
                    Serenity.ReflectionOptionsSetter.set(this.$widget, this.options);

                if (this.value != null)
                    Serenity.EditorUtils.setValue(this.$widget, this.value);

                if ($(this.$el).data('select2'))
                    this.$widget.changeSelect2(function () {
                        self.$emit('input', Serenity.EditorUtils.getValue(self.$widget));
                    });
                else
                    this.$widget.change(function () {
                        self.$emit('input', Serenity.EditorUtils.getValue(self.$widget));
                    });
            },
            destroyed: function () {
                if (this.$widget) {
                    this.$widget.destroy();
                    this.$widget = null;
                }
            }
        });
        return true;
    }

    // @ts-ignore
    !vueIntegration() && typeof $ !== "undefined" && $(vueIntegration);
}