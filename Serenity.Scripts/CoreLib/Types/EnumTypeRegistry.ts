﻿namespace Serenity {
    export namespace EnumTypeRegistry {

        let knownTypes: Q.Dictionary<Function>;

        export function tryGet(key: string): Function {

            if (knownTypes == null) {
                knownTypes = {};
                for (var type of Q.getTypes()) {
                    if (Q.isEnum(type)) {
                        var fullName = Q.getTypeFullName(type);
                        knownTypes[fullName] = type;
                        var enumKeyAttr = Q.getAttributes(type, Serenity.EnumKeyAttribute, false);
                        if (enumKeyAttr != null && enumKeyAttr.length > 0) {
                            knownTypes[enumKeyAttr[0].value] = type;
                        }

                        for (var k of Q.Config.rootNamespaces) {
                            if (Q.startsWith(fullName, k + '.')) {
                                knownTypes[fullName.substr(k.length + 1)] = type;
                            }
                        }
                    }
                }
            }

            if (knownTypes[key] == null)
                return null;

            return knownTypes[key];
        }

        export function get(key: string): Function {
            var type = EnumTypeRegistry.tryGet(key);
            if (type == null) {
                var message = Q.format("Can't find {0} enum type! If you have recently defined this enum type " + 
                    "in server side code, make sure your project builds successfully and transform T4 templates. " +
                    "Also make sure that enum is under your project root namespace, and your namespace parts starts " + 
                    "with capital letters, e.g.MyProject.Pascal.Cased namespace", key);

                Q.notifyError(message, '', null);
                throw new Q.Exception(message);
            }

            return type;
        }
    }
}