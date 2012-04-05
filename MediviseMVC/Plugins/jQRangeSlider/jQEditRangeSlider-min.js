/*
 jQRangeSlider
 A javascript slider selector that supports dates

 Copyright (C) Guillaume Gautreau 2012
 Dual licensed under the MIT or GPL Version 2 licenses.

*/
(function(d){d.widget("ui.editRangeSlider",d.ui.rangeSlider,{options:{type:"text",round:1},_inputs:[],_create:function(){d.ui.rangeSlider.prototype._create.apply(this);this.element.addClass("ui-editRangeSlider")},destroy:function(){this.element.removeClass("ui-editRangeSlider");d.ui.rangeSlider.prototype.destroy.apply(this)},_setOption:function(a,b){if(a==="type"&&this._isValidInputType(b))this.options.type=b,this.labels!=null&&(this._destroyLabels(),this._createLabels());else if(a==="round"&&this._isValidRoundValue(b))this.options.round=
b,this._fillInLabels();d.ui.rangeSlider.prototype._setOption.apply(this,[a,b])},_isValidInputType:function(a){return(a=="text"||a=="number")&&a!=this.options.type},_isValidRoundValue:function(a){return typeof a==="number"&&a>0||a===!1},_createLabel:function(a,b){if(a==null){var a=d.ui.rangeSlider.prototype._createLabel.apply(this,[a,b]),c=d("<input type='"+this.options.type+"' class='ui-editRangeSlider-inputValue' />").attr("name",b);this._inputs.push(c);this.options.type==="number"&&c.click(d.proxy(this._onChange,
this));c.keyup(d.proxy(this._onKeyUp,this));c.blur(d.proxy(this._onChange,this));a.append(c)}return a},_onChange:function(){if(this._inputs.length==2){var a=this._returnCheckedValue(this._inputs[0].val()),b=this._returnCheckedValue(this._inputs[1].val());a!==!1&&b!==!1&&(a=this._returnCheckedValues(a,b),this.values(a.min,a.max))}},_returnCheckedValue:function(a){var b=parseFloat(a);if(isNaN(b)||b.toString()!=a)return!1;b=Math.min(this.options.bounds.max,b);return b=Math.max(this.options.bounds.min,
b)},_returnCheckedValues:function(a,b){var c={min:Math.min(a,b),max:Math.max(a,b)},d=c.max-c.min,f=this.values(),e=!1;Math.abs(c.min-f.min)<Math.abs(c.max-f.max)&&(e=!0);if(this.options.range.min!==!1&&this.options.range.min>d)e?c.max=c.min+this.options.range.min:c.min=c.max-this.options.range.min;else if(this.options.range.max!==!1&&this.options.range.max<d)e?c.max=c.min+this.options.range.max:c.min=c.max-this.options.range.max;return c},_onKeyUp:function(a){if(a.which==13)return this._onChange(a),
!1},_destroyLabels:function(){this._inputs=[];d.ui.rangeSlider.prototype._destroyLabels.apply(this)},_fillInLabel:function(a,b){a.find("input").val(this._format(b))},_defaultFormat:function(a){if(this.options.round!==!1)return Math.round(a/this.options.round)*this.options.round;return a}})})(jQuery);
