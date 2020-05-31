﻿namespace Serenity {

    @Serenity.Decorators.registerInterface()
    export class IStringValue {
    }

    export interface IStringValue {
        get_value(): string;
        set_value(value: string): void;
    }
}