﻿
namespace Slick.Aggregators
{
    export function Avg(field: string): void {
        this.field_ = field;

        this.init = function () {
            this.count_ = 0;
            this.nonNullCount_ = 0;
            this.sum_ = 0;
        };

        this.accumulate = function (item: any) {
            var val = item[this.field_];
            this.count_++;
            if (val != null && val !== "" && !isNaN(val)) {
                this.nonNullCount_++;
                this.sum_ += parseFloat(val);
            }
        };

        this.storeResult = function (groupTotals: any) {
            if (!groupTotals.avg) {
                groupTotals.avg = {};
            }
            if (this.nonNullCount_ != 0) {
                groupTotals.avg[this.field_] = this.sum_ / this.nonNullCount_;
            }
        };
    }

    export function WeightedAvg(field: string, weightedField: string) {
        this.field_ = field;
        this.weightedField_ = weightedField;

        this.init = function () {
            this.sum_ = 0;
            this.weightedSum_ = 0;
        };

        this.accumulate = function (item: any) {
            var val = item[this.field_];
            var valWeighted = item[this.weightedField_];
            if (this.isValid(val) && this.isValid(valWeighted)) {
                this.weightedSum_ += parseFloat(valWeighted);
                this.sum_ += parseFloat(val) * parseFloat(valWeighted);
            }
        };

        this.storeResult = function (groupTotals: any) {
            if (!groupTotals.avg) {
                groupTotals.avg = {};
            }

            if (this.sum_ && this.weightedSum_) {
                groupTotals.avg[this.field_] = this.sum_ / this.weightedSum_;
            }
        };

        this.isValid = function (val: any) {
            return val !== null && val !== "" && !isNaN(val);
        };
    }

    export function Min(field: string): void {
        this.field_ = field;

        this.init = function () {
            this.min_ = null;
        };

        this.accumulate = function (item: any) {
            var val = item[this.field_];
            if (val != null && val !== "" && !isNaN(val)) {
                if (this.min_ == null || val < this.min_) {
                    this.min_ = val;
                }
            }
        };

        this.storeResult = function (groupTotals: any) {
            if (!groupTotals.min) {
                groupTotals.min = {};
            }
            groupTotals.min[this.field_] = this.min_;
        }
    }

    export function Max(field: string): void {
        this.field_ = field;

        this.init = function () {
            this.max_ = null;
        };

        this.accumulate = function (item: any) {
            var val = item[this.field_];
            if (val != null && val !== "" && !isNaN(val)) {
                if (this.max_ == null || val > this.max_) {
                    this.max_ = val;
                }
            }
        };

        this.storeResult = function (groupTotals: any) {
            if (!groupTotals.max) {
                groupTotals.max = {};
            }
            groupTotals.max[this.field_] = this.max_;
        }
    }

    export function Sum(field: string): void {
        this.field_ = field;

        this.init = function () {
            this.sum_ = null;
        };

        this.accumulate = function (item: any) {
            var val = item[this.field_];
            if (val != null && val !== "" && !isNaN(val)) {
                this.sum_ += parseFloat(val);
            }
        };

        this.storeResult = function (groupTotals: any) {
            if (!groupTotals.sum) {
                groupTotals.sum = {};
            }
            groupTotals.sum[this.field_] = this.sum_;
        }
    }
}